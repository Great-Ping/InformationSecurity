using System.Buffers;
using System.ComponentModel;
using System.Globalization;

namespace InformationSecurity.Cryptography.DataEncryptedStandard;

public static class DesHelper
{
    public const byte BitsInByte = 8; 
    public const int LastBitIndex = BitsInByte - 1;
    public const int BlockSizeBytes = 8;

    public static void BlockEncryptionProcess(
        Span<byte> originalBlock,
        Span<byte> newBlock,
        DataEncryptedStandardOptions desOptions,
        IEnumerable<byte[]> keys,
        int leftStart,
        int rightStart
    )
    {
        //Начальная перестановка
        DesHelper.ShuffleBits(originalBlock, newBlock, desOptions.InitialPermutations);

        Span<byte> leftPart = newBlock.Slice(leftStart, BlockSizeBytes / 2);
        Span<byte> rightPart = newBlock.Slice(rightStart, BlockSizeBytes / 2);
        Span<byte> extendsBlock = originalBlock.Slice(0, 6);

        byte[] buffer = ArrayPool<byte>.Shared.Rent(rightPart.Length * 3);
        Span<byte> newRightPart = buffer.AsSpan(0, rightPart.Length);

        foreach (byte[] key in keys)
        {
            DesHelper.ExtenedsBits(
                rightPart,
                extendsBlock,
                desOptions.MessageExtends48
            );

            DesHelper.Xor(key, extendsBlock);

            DesHelper.STransformation(extendsBlock, newRightPart, desOptions.STransformations);


            Span<byte> shuffledBlock = extendsBlock.Slice(0, newRightPart.Length);
            DesHelper.ShuffleBits(newRightPart, shuffledBlock, desOptions.PFuncPermutation);

            DesHelper.Xor(shuffledBlock, leftPart, outBlock: newRightPart);

            Span<byte> temp = leftPart;
            leftPart = rightPart;
            rightPart = newRightPart;
            newRightPart = temp;
        }

        Span<byte> result = buffer.AsSpan(newRightPart.Length, leftPart.Length + rightPart.Length);

        leftPart.CopyTo(result.Slice(leftStart, leftPart.Length));
        rightPart.CopyTo(result.Slice(rightStart, rightPart.Length));

        //Конечная перестановка
        DesHelper.ShuffleBits(result, newBlock, desOptions.FinalPermutations);

        ArrayPool<byte>.Shared.Return(buffer);
    }

    /// <summary>
    /// Используется как для перестановок так и для сжатия
    /// </summary>
    private static void  CommonShuffleBits(
        ReadOnlySpan<byte> originalBlock, 
        Span<byte> newBlock,
        ReadOnlySpan<byte> permutations,
        int length
    ) {

        for (int i = 0; i < length; i++)
        {
            newBlock[i] &= 0b0;

            int absoluteI = i * BitsInByte;
            for (int j = 0; j < BitsInByte; j++)
            {
                byte oldPosition = permutations[absoluteI + j];
        
                int oldI = oldPosition / BitsInByte;
                int oldJ = oldPosition % BitsInByte;
                // Нормирование J из за способа установки бита
                // realJ = BitsInByte - 1 - j = outBitsInByte - j
                int bitValue = (originalBlock[oldI] >> (LastBitIndex - oldJ)) & 0b1; 
                newBlock[i] |= (byte)(bitValue << (LastBitIndex - j));
            }
        }
    }

    public static void ShuffleBits(
        ReadOnlySpan<byte> originalBlock,
        Span<byte> newBlock,
        ReadOnlySpan<byte> permutations
    )
    {
        CommonShuffleBits(
            originalBlock, 
            newBlock, 
            permutations, 
            originalBlock.Length
        );
    }

    public static void CompressBits(
        ReadOnlySpan<byte> originalBlock, 
        Span<byte> newBlock,
        ReadOnlySpan<byte> permutations
    ) {
        CommonShuffleBits(
            originalBlock,
            newBlock,
            permutations,
            newBlock.Length
        );
    }

    public static void ExtenedsBits(
        ReadOnlySpan<byte> originalBlock, 
        Span<byte> newBlock,
        ReadOnlySpan<byte> permutations
    ) {
        CommonShuffleBits(
            originalBlock,
            newBlock,
            permutations,
            newBlock.Length
        );
    }

    public static void BitsShift(
        ReadOnlySpan<byte> originalBlock, 
        Span<byte> newBlock,
        int shift
    ) {
        newBlock[0] = (byte)(originalBlock[0] << shift);
        for (int i = 1; i < originalBlock.Length; i++)
        {
            byte remainder = (byte)(originalBlock[i] >> (BitsInByte - shift));
            newBlock[i] = (byte)(originalBlock[i] << shift);
            newBlock[i - 1] |= remainder;
        }
    }

    public static void Xor(ReadOnlySpan<byte> key, Span<byte> block)
    {
        for (int i = 0; i < block.Length; i++)
        { 
            block[i] = (byte)(key[i] ^ block[i]);
        }
    }

    public static void Xor(ReadOnlySpan<byte> key, ReadOnlySpan<byte> block, Span<byte> outBlock)
    {
        for (int i = 0; i < block.Length; i++)
        {
            outBlock[i] = (byte)(key[i] ^ block[i]);
        }
    }


    /// <summary>
    /// Циклический сдвиг двух половинок ключа
    /// |LR|000 |RR|000 
    /// shift 1
    /// 000|LR| 000|RR|
    /// </summary>
    public static void KeyCyclicShift(
        ReadOnlySpan<byte> originalBlock, 
        Span<byte> newBlock,
        int shift
    ) {
       BitsShift(originalBlock, newBlock, shift);
       
       byte leftRemainder = (byte)(originalBlock[0] >> (BitsInByte - shift));
       int middle = (BitsInByte / 2) * originalBlock.Length;

       int byteIndex = middle / BitsInByte;     // i 
       int bitIndex = middle % BitsInByte;      // j
       
       byte rightRemainder = (byte)((newBlock[byteIndex] >> bitIndex) & ~(byte.MaxValue << shift));
       
       newBlock[byteIndex] ^= (byte)(rightRemainder << bitIndex);
       newBlock[byteIndex] |= (byte)(leftRemainder << bitIndex);
       
       newBlock[^1] |= rightRemainder;
    }

    public static List<byte[]> PrepareFeistelKeys(byte[] sourceKey, DataEncryptedStandardOptions desOptions)
    {
        List<byte[]> result = new(16);
        byte[] bitBuffer = ArrayPool<byte>.Shared.Rent(14); 
        Span<byte> generationKey = bitBuffer.AsSpan(0, 7);
        Span<byte> newGenerationKey = bitBuffer.AsSpan(8, 7);
        
        //Получаем 56 битный ключ
        DesHelper.CompressBits(
            sourceKey,
            generationKey,
            desOptions.KeyСompression56
        );
        
        //Формируем ключи для раундов
        for (int i = 0; i < desOptions.GenerationKeyShifts.Length; i++)
        {
            //48 битный ключ для шифрования
            byte[] key = new byte[6];
            
            DesHelper.KeyCyclicShift(
                generationKey,
                newGenerationKey,
                desOptions.GenerationKeyShifts[i]
            );
            
            DesHelper.CompressBits(
                newGenerationKey,
                    key,
                desOptions.KeyСompression48
            );
            
            result.Add(key);
            
            //Свапаем буферы местами, чтоб новый 56 битный ключ, стал текущим
            Span<byte> temp = newGenerationKey;
            newGenerationKey = generationKey;
            generationKey = temp;
            
        }
        
        ArrayPool<byte>.Shared.Return(bitBuffer);
        return result;
    }

    public static void WriteBits(Span<byte> block, byte word, int startBitIndex, int wordLengthBits)
    {
        int byteIndex = startBitIndex / 8;
        int bitInByteIndex = startBitIndex % 8;
        
        int startBitShift = BitsInByte - bitInByteIndex;
        int wordLeftShift = startBitShift - wordLengthBits;
        byte wordMask = (byte)(byte.MaxValue << startBitShift);

        int normalizedWorld;

        if (wordLeftShift >= 0)
        {
            normalizedWorld = word << wordLeftShift;
            wordMask |= (byte)(~(byte.MaxValue << wordLeftShift));
        }
        else
        {
            normalizedWorld = word >> -wordLeftShift;
            WriteBits(
                block, 
                word, 
                startBitIndex + (wordLengthBits + wordLeftShift) ,
                -wordLeftShift
            );
        }
        
        block[byteIndex] &= wordMask; 
        block[byteIndex] |= (byte)(normalizedWorld);
    }
    
    public static byte ReadBits(ReadOnlySpan<byte> block, int startBitIndex, int wordLengthBits)
    {   
        int byteIndex = startBitIndex / 8;
        int bitInByteIndex = startBitIndex % 8;
        
        int startBitShift = BitsInByte - bitInByteIndex; 
        int wordLeftShift = startBitShift - wordLengthBits;
        byte wordMask = (byte)(~(byte.MaxValue << startBitShift));

        byte word = 0;
        
        if (wordLeftShift >= 0)
        {
            word = (byte)(block[byteIndex] >> wordLeftShift);
        }
        else
        {
            word = (byte)((block[byteIndex] << -wordLeftShift) | ReadBits(
                block, 
                startBitIndex + (wordLengthBits + wordLeftShift) ,
                -wordLeftShift
            )); 
        }

        return word;
    }

    
    public static void STransformation(
        Span<byte> sourceBlock, 
        Span<byte> newBlock, 
        IEnumerable<byte[,]> STransformations
    )
    {
        const int sourceBitsLength = 6;
        const int newBitsLength = 4;
        const int rowMask    = 0b100001;
        const int columnMask = 0b011110;
        
        int sourceBlockStartIndex = 0;
        int newBlockStartIndex = 0;
        foreach (byte[,] sBits in STransformations)
        {
            byte bits = DesHelper.ReadBits(
                sourceBlock,
                sourceBlockStartIndex,
                sourceBitsLength
            );
            
            int rowBits = bits & rowMask;
            int columnBits = bits & columnMask;

            int columnIndex = columnBits >> 1;
            int rowIndex = (rowBits >> 4) + (rowBits & 0b1);
            
            DesHelper.WriteBits(
                newBlock,
                sBits[rowIndex, columnIndex],
                newBlockStartIndex,
                newBitsLength
            ); 
            
            newBlockStartIndex += newBitsLength;
            sourceBlockStartIndex += sourceBitsLength;
        }
        
           
    }
    
    
}