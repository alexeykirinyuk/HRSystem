import {IAttributeService} from "../../core/IAttributeService";

export interface ISaveAttributeProps {
    id?: number;
    service: IAttributeService;
    show: boolean;
    onHide: () => void;
}