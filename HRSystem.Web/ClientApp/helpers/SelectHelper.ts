import {Option} from "react-select";

export class SelectHelper {
    public static getValue<T>(options: Option<T>): T {
        return options.value;
    }
}