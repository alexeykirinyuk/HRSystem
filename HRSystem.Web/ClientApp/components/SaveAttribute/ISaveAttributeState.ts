import {ISaveAttributeParams} from "../../core/IAttributeService";
import {Option} from "react-select";
import {AttributeType} from "../../models/AttributeType";
import { ValidationError } from "../../models/ValidationErrors";

export interface ISaveAttributeState extends ISaveAttributeParams {
    show: boolean;
    attributeTypeOption: Option<AttributeType>;
    isLoading: boolean;
    validationErrors: ValidationError[];
}