using System.Net;

namespace Common.Exceptions
{

    public enum ApiExceptionType
    {
        [Error("1", HttpStatusCode.OK, Message = "Successfully")]
        Ok,
        [Error("10", HttpStatusCode.BadRequest, Message = "Record no found")]
        BadRequest,

        [Error("11", HttpStatusCode.NotFound, Message = "Record no found")]
        NotFound,

        [Error("700", HttpStatusCode.BadRequest, Message = "Wrong Voucher Number")]
        WrongVoucherNumber,

        [Error("701", HttpStatusCode.BadRequest, Message = "Voucher Is Not Active")]
        VoucherIsNotActive,

        [Error("702", HttpStatusCode.BadRequest, Message = "Voucher Already Used")]
        VoucherAlreadyUsed,

        [Error("409", HttpStatusCode.BadRequest, Message = "Code Already Used")]
        CodeAlreadyUsed,

        [Error("410", HttpStatusCode.Forbidden, Message = "Code is Expired")]
        CodeExpired,

        [Error("411", HttpStatusCode.Forbidden, Message = "Code is not active")]
        CodeIsNotActive,

        [Error("20", HttpStatusCode.BadRequest, Message = "Invalid login")]
        InvalidLogin,
        [Error("30", HttpStatusCode.BadRequest, Message = "NationalId must be unique")]
        UniqueNationalId,
        [Error("31", HttpStatusCode.BadRequest, Message = "Phone Number must be unique")]
        UniquePhoneNumber,

        [Error("40", HttpStatusCode.BadRequest, Message = "Validation error ")]
        ValidationError,

        [Error("50", HttpStatusCode.BadRequest, Message = "The record you attempt to delete have a lot of dependencies ")]
        DeleteRelatedObjectError,

        [Error("51", HttpStatusCode.Forbidden, Message = "Not Authorized to access this method")]
        Forbidden,
        [Error("52", HttpStatusCode.Unauthorized, Message = "Not Authorized to access this method")]
        Unauthorized,







        //Files errors
        [Error("100", HttpStatusCode.UnsupportedMediaType, Message = "Invalid Image file formate")]
        InvalidImage,
        [Error("101", HttpStatusCode.UnsupportedMediaType, Message = "Faild to delete image form cache directory")]
        FaildDeleteImage,
        [Error("102", HttpStatusCode.UnsupportedMediaType, Message = "Faild to save image")]
        FaildSaveImage,

        [Error("99", HttpStatusCode.Unauthorized, Message = "Not Authorized to access this method")]
        RequestInProgress = 99,

        [Error("98", HttpStatusCode.Unauthorized, Message = "Request already new")]
        NewRequest = 98,
    }
}
