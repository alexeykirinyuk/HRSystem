export class EventHelper {
    private constructor() {}

    public static getValue(event: any): string {
        return event.target.value as string;
    }
}