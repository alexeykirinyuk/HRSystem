import { StringHelper } from "./StringHelper";

export class EventHelper {
    private constructor() {}

    public static getValue(event: any): string {
        if (event.target == null) {
            return StringHelper.EMPTY;
        }

        return event.target.value as string;
    }

    public static getBoolValue(event: any): string {
        if (event.target == null) {
            return "false";
        }

        return event.target.checked.toString();
    }
}