import {ISaveAttributeParams} from "../../core/IAttributeService";
import {Option} from "react-select";
import {AttributeType} from "../../models/AttributeType";

export interface ISaveAttributeState extends ISaveAttributeParams {
    show: boolean;
    onHide: () => void;
    attributeTypeOption: Option<AttributeType>;
    isLoading: boolean;
}