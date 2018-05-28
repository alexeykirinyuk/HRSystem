import {Employee} from "../../models/Employee";
import {AttributeInfo} from "../../models/AttributeInfo";
import {Option} from "react-select";
import {IAddEmployeeParams} from "../../core/IEmployeeService";

export interface ISaveEmployeeState extends IAddEmployeeParams {
    attributesInfo: Array<AttributeInfo>;
    isLoading: boolean;
    employees: Employee[];
    show: boolean;
    onHide: () => void;
    managerLoginOption: Option<string>;
}