import { AttributeInfo } from "../../models/AttributeInfo";

export interface IAttributeControlProps {
    info: AttributeInfo;
    placeholder: string;
    value: string;
    onChange: (info: AttributeInfo, value: string) => void;
}