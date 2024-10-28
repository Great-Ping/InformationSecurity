using System.Buffers;
using System.Buffers.Text;
using System.Collections;
using System.Text;

using static InformationSecurity.Cryptography.DataEncryptedStandard.DesHelper;

namespace InformationSecurity.Cryptography.DataEncryptedStandard;

public class ElectronicCodeBookCryptographer
    : BaseCryptographer<ElectronicCodeBookCryptographerOptions>
{
    private readonly Encoding _encoding;

    
    public ElectronicCodeBookCryptographer() 
        : base(ElectronicCodeBookCryptographerOptions.Default)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        _encoding = Encoding.GetEncoding(1251);
    }

    public override char[] Encrypt(ReadOnlySpan<char> message)
    {
        return Process(
            message, 
            Options.DataEncryptedStandard,
            Options.FeistelKeys,
            0,
            BlockSizeBytes / 2
        );
    }

    public override char[] Decrypt(ReadOnlySpan<char> encrypted)
    {
        return Process(
            encrypted, 
            Options.DataEncryptedStandard.Reverse(),
            Options.FeistelKeys.Reverse(),
            BlockSizeBytes / 2,
            0
        );
    }

    private char[] Process(
        ReadOnlySpan<char> message, 
        DataEncryptedStandardOptions desOptions,
        IEnumerable<byte[]> feistelKeys,
        int leftStart,
        int rightStart
    ){
        
        int messageByteCount = _encoding.GetByteCount(message);
        
        //Подсчитываем выравнивание буффера
        int wholeBlocksCount = messageByteCount / BlockSizeBytes;
        bool hasRemainder = messageByteCount % BlockSizeBytes != 0;
        
        int messageBufferLength = (hasRemainder) 
            ? (wholeBlocksCount + 1) * BlockSizeBytes
            : messageByteCount;

        //Общий буффер, на все случаи жизни
        byte[] commonBuffer = new byte[
            messageBufferLength 
            + BlockSizeBytes * 2
        ];
        
        Span<byte> messageBuffer = commonBuffer.AsSpan(0, messageBufferLength);
        Span<byte> originalBlockBuffer = commonBuffer.AsSpan(messageBufferLength, BlockSizeBytes);
        int newBlockBufferStart = messageBufferLength + BlockSizeBytes;
        

        //Первые байты заполняем нулями
        int usefulBitsStart = messageBufferLength - messageByteCount;
        messageBuffer
            .Slice(0, usefulBitsStart)
            .Fill(0);
        
        _encoding.GetBytes(
            message, 
            messageBuffer.Slice(
                usefulBitsStart, 
                messageByteCount
            )
        );
            
        
        for (int i = 0; i < message.Length; i += BlockSizeBytes)
        {
            Span<byte> newBlockBuffer = commonBuffer.AsSpan(i, originalBlockBuffer.Length);
            //Перезапишем буффер для байт сообщения 
            //Т.к. в результате хранятся байты исходной строки
            newBlockBuffer.CopyTo(originalBlockBuffer);
                
            DesHelper.BlockEncryptionProcess(
                originalBlockBuffer,
                newBlockBuffer,
                desOptions,
                feistelKeys,
                leftStart,
                rightStart
            );
        }



        //Затираем начальные нули
        int zerosCount = 0;
        while (messageBuffer[zerosCount] == 0) {
            zerosCount++;
        }

        Span<byte> result = messageBuffer.Slice(zerosCount);

        int responseLength = _encoding.GetCharCount(result);
        char[] response = new char[responseLength];

        _encoding.GetChars(result, response);

        return response;
        
    }
    

}