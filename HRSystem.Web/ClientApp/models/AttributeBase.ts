import {Employee} from "./Employee";
import {AttributeInfo} from "./AttributeInfo";

export class AttributeBase {
    public id: number;
    public employee: Employee;
    public attributeInfo: AttributeInfo;

    private value: any;

    public GetValue<T>(): T {
        return this.value;
    }

    public SetValue<T>(value: T): void {
        this.value = value;
    }
}