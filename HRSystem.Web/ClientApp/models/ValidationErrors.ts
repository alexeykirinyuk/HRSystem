export class ValidationErrors {
    public static readonly ERROR_TYPE: string = "ValidationError";

    public static parse(error: any): Array<ValidationError> {
        let responseData = error.response.data;
        if (responseData.errorType == "ValidationError") {
            return responseData.errors.map((e: any) => new ValidationError(e as ValidationError));
        }

        throw error;
    }

    public static isValidationError(error: any): boolean {
        return error.response.status == 400 && error.response.data.errorType == ValidationErrors.ERROR_TYPE;
    }
}

export class ValidationError {
    message: string;

    public constructor(error: ValidationError) {
        this.message = error.message;
    }
}