export class RequestUrls {
    private constructor() {}

    private static readonly API = "api";
    private static readonly EMPLOYEE = "employee";
    private static readonly ATTRIBUTE = "attribute";
    private static readonly DOCUMENT = "document";

    public static readonly GET_ALL_EMPLOYEES = `${RequestUrls.API}/${RequestUrls.EMPLOYEE}/all`;
    public static readonly GET_EMPLOYEE_CREATION_INFO = `${RequestUrls.API}/${RequestUrls.EMPLOYEE}/creationInfo`;
    public static readonly SAVE_EMPLOYEE = `${RequestUrls.API}/${RequestUrls.EMPLOYEE}/save`;

    public static readonly GET_ALL_ATTRIBUTES = `${RequestUrls.API}/${RequestUrls.ATTRIBUTE}/all`;
    public static readonly ADD_ATTRIBUTE = `${RequestUrls.API}/${RequestUrls.ATTRIBUTE}/save`;
    public static readonly GET_ATTRIBUTE_SAVING_INFO = `${RequestUrls.API}/${RequestUrls.ATTRIBUTE}/savingInfo`;
    public static readonly DELETE_ATTRIBUTE = `${RequestUrls.API}/${RequestUrls.ATTRIBUTE}/delete`;

    public static readonly UPLOAD_DOCUMENT = `${RequestUrls.API}/${RequestUrls.DOCUMENT}/upload`;
    public static readonly DELETE_DOCUMENT = `${RequestUrls.API}/${RequestUrls.DOCUMENT}/delete`;
}