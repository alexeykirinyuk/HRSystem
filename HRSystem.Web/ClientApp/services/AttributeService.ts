import {ISaveAttributeParams, IAttributeService, IAttributeSavingInfoQueryResponse} from "../core/IAttributeService";
import {IDataService} from "../core/IDataService";
import {RequestUrls} from "./RequestUrls";
import {GetAllAttributesResponse} from "../core/IEmployeeService";
import {StringHelper} from "../helpers/StringHelper";

export class AttributeService implements IAttributeService {
    constructor(private dataService: IDataService) {}

    public getAll(): Promise<GetAllAttributesResponse> {
        return this.dataService.makeGetRequest(RequestUrls.GET_ALL_ATTRIBUTES);
    }

    public save(request: ISaveAttributeParams): Promise<void> {
        return this.dataService.makePostRequest(RequestUrls.ADD_ATTRIBUTE, request);
    }

    getSavingInfo(id?: number): Promise<IAttributeSavingInfoQueryResponse> {
        let idString = id != null ? id.toString() : StringHelper.EMPTY;

        return this.dataService.makeGetRequest(`${RequestUrls.GET_ATTRIBUTE_SAVING_INFO}/${idString}`);
    }
}