﻿namespace eCommerce.API.DTO;

public class ColorPostDTO
{
    public string Name { get; set; } = string.Empty;
    public string TextColor { get; set; } = string.Empty;
    public string BGColor { get; set; } = string.Empty;
    //public OptionType OptionType { get; set; }
}

public class ColorPutDTO : ColorPostDTO
{
    public int Id { get; set; }
}

public class ColorGetDTO : ColorPutDTO
{
    public bool IsSelected { get; set; }
}


