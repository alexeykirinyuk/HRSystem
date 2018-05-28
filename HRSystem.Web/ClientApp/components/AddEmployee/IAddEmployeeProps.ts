import {IEmployeeService} from "../../core/IEmployeeService";

export interface IAddEmployeeProps {
    service: IEmployeeService;
    show: boolean;
    onHide: () => void;
}