import {IDataService} from "../core/IDataService";
import axios, { AxiosRequestConfig } from "axios";
import {throws} from "assert";

export class DataService implements IDataService {
    public constructor(private baseUrl: string) {
    }

    private castUrl(url: string): string {
        return `${this.baseUrl}/${url}`;
    }

    public makeGetRequest<T>(url: string): Promise<T> {
        let requestUrl = this.castUrl(url);
        console.log(`Making get request to ${requestUrl}`);

        return axios.get<T>(requestUrl).then(value => value.data)
    }

    public makePostRequest<T>(url: string, body: any, config?: AxiosRequestConfig): Promise<T> {
        return axios.post(this.castUrl(url), body, config)
            .then(v => v.data)
            .catch(e => {
                throw e;
            });
    }
}