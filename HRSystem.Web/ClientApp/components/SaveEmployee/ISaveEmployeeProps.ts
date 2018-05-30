import {IEmployeeService} from "../../core/IEmployeeService";

export interface ISaveEmployeeProps {
    employeeService: IEmployeeService;
    show: boolean;
    onHide: () => void;
    isCreate: boolean;
    login?: string;
}