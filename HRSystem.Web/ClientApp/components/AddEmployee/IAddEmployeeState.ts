import {Employee} from "../../models/Employee";
import {AttributeInfo} from "../../models/AttributeInfo";
import {IEmployee} from "../../core/IEmployee";
import {Option} from "react-select";
import {IAddEmployeeParams} from "../../core/IEmployeeService";

export interface IAddEmployeeState extends IAddEmployeeParams {
    attributesInfo: Array<AttributeInfo>;
    isLoading: boolean;
    employees: Employee[];
    show: boolean;
    onHide: () => void;
    managerOption: Option<Employee>;
}