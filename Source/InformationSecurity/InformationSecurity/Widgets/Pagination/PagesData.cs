using System;
using System.Collections.Generic;

namespace InformationSecurity.Shared.Pagination;

public record PageData (
    ViewModelBase ViewModel,
    Type ViewType
){
    
}