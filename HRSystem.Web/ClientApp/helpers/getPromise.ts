import {AxiosPromise} from "axios";

export class AxiosHelper {
    private constructor() {}

    public static getPromise<T>(axiosPromise: AxiosPromise<T>): Promise<T> {
        return new Promise<T>(((resolve, reject) => {

        }));
    }
}