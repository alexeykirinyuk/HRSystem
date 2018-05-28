import {Employee} from "./Employee";
import {AttributeInfo} from "./AttributeInfo";

export class AttributeBase {
    public id: number;
    public attributeInfo: AttributeInfo;
    public value: string;

    public constructor(params?: AttributeBase) {
        if (params == null) {
            return;
        }

        this.id = params.id;

        if (params.attributeInfo != null) {
            this.attributeInfo = new AttributeInfo(params.attributeInfo);
        }
        this.value = params.value;
    }
}