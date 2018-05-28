import {IEmployeeService} from "../../core/IEmployeeService";

export interface ISaveEmployeeProps {
    service: IEmployeeService;
    show: boolean;
    onHide: () => void;
    isCreate: boolean;
    login?: string;
}