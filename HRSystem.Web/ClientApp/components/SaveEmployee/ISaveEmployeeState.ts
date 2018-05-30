import {Employee} from "../../models/Employee";
import {AttributeInfo} from "../../models/AttributeInfo";
import {Option} from "react-select";
import {ISaveEmployeeParams} from "../../core/IEmployeeService";
import { ValidationError } from "../../models/ValidationErrors";

export interface ISaveEmployeeState extends ISaveEmployeeParams {
    attributesInfo: Array<AttributeInfo>;
    isLoading: boolean;
    employees: Employee[];
    show: boolean;
    managerLoginOption: Option<string>;
    validationErrors: ValidationError[];
}