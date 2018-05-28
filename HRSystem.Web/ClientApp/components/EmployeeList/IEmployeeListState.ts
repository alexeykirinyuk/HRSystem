import {Employee} from "../../models/Employee";
import {AttributeInfo} from "../../models/AttributeInfo";

export interface IEmployeeListState {
    employees: Array<Employee>;
    attributes: Array<AttributeInfo>;
    isLoading: boolean;
    showModal: boolean;
}