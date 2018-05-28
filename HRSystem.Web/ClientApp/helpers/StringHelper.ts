export class StringHelper {
    public static readonly EMPTY = "";

    public static isNullOrEmpty(str: string): boolean {
        return str == null || str == StringHelper.EMPTY;
    }
}