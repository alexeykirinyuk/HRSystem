import {IEmployeeListProps} from "./IEmployeeListProps";
import {IEmployeeListState} from "./IEmployeeListState";
import * as React from "react";
import {Button, ControlLabel, FormControl, FormGroup, Label, Table} from "react-bootstrap/lib";
import {Spinner} from "../Spinner/Spinner";
import {SaveEmployee} from "../SaveEmployee/SaveEmployee";
import {StringHelper} from "../../helpers/StringHelper";
import {Employee} from "../../models/Employee";
import {AttributeType} from "../../models/AttributeType";
import {EventHelper} from "../../helpers/EventHelper";
import "./EmployeeList.scss"
import Select, {Option} from "react-select";

export class EmployeeList extends React.Component<IEmployeeListProps, IEmployeeListState> {
    public constructor(props: IEmployeeListProps) {
        super(props);

        this.state = {
            employees: [],
            attributes: [],
            isLoading: true,
            showModal: false,
            isCreateModalType: true,
            selectedEmployeeLogin: StringHelper.EMPTY,
            search: StringHelper.EMPTY,
            searchManagers: [],
            searchManagerSelected: StringHelper.EMPTY,
            searchManagerSelectedOption: null,
            searchOffices: [],
            searchOfficeSelected: StringHelper.EMPTY,
            searchOfficeSelectedOption: null,
            searchJobTitles: [],
            searchJobTitleSelected: StringHelper.EMPTY,
            searchJobTitleSelectedOption: null,
            searchAttributeValues: new Map<number, string>()
        };
    }

    public render(): React.ReactElement<IEmployeeListProps> {
        let managerOptions: Option<string>[] = this.getOptions(this.state.searchManagers);
        let officeOptions: Option<string>[] = this.getOptions(this.state.searchOffices);
        let jobTitleOptions: Option<string>[] = this.getOptions(this.state.searchJobTitles);

        let attributeNameElements: JSX.Element[]
            = this.state.attributes.map(a => <th key={a.name}>{a.name}</th>);
        let employeeElements: JSX.Element[] = this.state.employees.map(e =>
            (<tr key={e.login}>
                <td>{e.fullName}</td>
                <td>{e.email}</td>
                <td>{e.office}</td>
                <td>{e.phone}</td>
                <td>{e.jobTitle}</td>
                <td>{e.manager != null ? e.manager.fullName : ""}</td>
                {this.getEmployeeAttributes(e)}
                <td>
                    <Button
                        bsStyle="success"
                        onClick={() =>
                            this.setState({showModal: true, isCreateModalType: false, selectedEmployeeLogin: e.login})}>
                        UPDATE</Button>
                </td>
            </tr>));

        return (<div className="employeeList">
            <div className="employeeListInner">
                <SaveEmployee
                    employeeService={this.props.employeeService}
                    show={this.state.showModal}
                    onHide={() => this.hideModal()}
                    isCreate={this.state.isCreateModalType}
                    login={this.state.selectedEmployeeLogin}/>
                <div className="commandBlock">
                    <div className="commandBlockInner">
                        <Button
                            bsStyle="success"
                            className="createNewButton"
                            onClick={() => this.setState({showModal: true, isCreateModalType: true})}>CREATE
                            NEW</Button>
                        <FormGroup className="searchFormGroup">
                            <div className="searchLabelWrapper">
                                <ControlLabel className="searchLabel">Manager</ControlLabel>
                            </div>
                            <div className="searchSelectWrapper">
                                <Select className="searchSelect"
                                        value={this.state.searchManagerSelectedOption}
                                        onChange={option => this.setState({
                                            searchManagerSelected: option == null ? managerOptions[0].value : (option as Option<string>).value,
                                            searchManagerSelectedOption: option == null ? managerOptions[0] : option
                                        })}
                                        options={managerOptions}
                                        placeholder="Select manager for searching...">
                                </Select>
                            </div>
                        </FormGroup>
                        <FormGroup className="searchFormGroup">
                            <div className="searchLabelWrapper">
                                <ControlLabel className="searchLabel">Office</ControlLabel>
                            </div>
                            <div className="searchSelectWrapper">
                                <Select className="searchSelect"
                                        value={this.state.searchOfficeSelectedOption}
                                        onChange={option => this.setState({
                                            searchOfficeSelected: option == null ? officeOptions[0].value : (option as Option<string>).value,
                                            searchOfficeSelectedOption: option == null ? officeOptions[0] : option
                                        })}
                                        options={officeOptions}
                                        placeholder="Select office for searching...">
                                </Select>
                            </div>
                        </FormGroup>
                        <FormGroup className="searchFormGroup">
                            <div className="searchLabelWrapper">
                                <ControlLabel className="searchLabel">Job Title</ControlLabel>
                            </div>
                            <div className="searchSelectWrapper">
                                <Select className="searchSelect"
                                        value={this.state.searchJobTitleSelectedOption}
                                        onChange={option => this.setState({
                                            searchJobTitleSelected: option == null ? jobTitleOptions[0].value : (option as Option<string>).value,
                                            searchJobTitleSelectedOption: option == null ? jobTitleOptions[0] : option
                                        })}
                                        options={jobTitleOptions}
                                        placeholder="Select job titles for searching...">
                                </Select>
                            </div>
                        </FormGroup>
                        {
                            this.state.attributes
                                .filter(a => a.type != AttributeType.Document)
                                .map(a =>
                                    <FormGroup className="searchFormGroup">
                                        <div className="searchLabelWrapper">
                                            <ControlLabel>{a.name}</ControlLabel>
                                        </div>
                                        <div className="searchSelectWrapper">
                                            <FormControl
                                                className="searchSelect"
                                                type="text"
                                                onChange={(event) => this.setSearchAttributeValue(a.id, EventHelper.getValue(event))}
                                                placeholder="Enter search request"
                                            />
                                        </div>
                                    </FormGroup>)
                        }
                        <FormGroup className="searchFormGroup">
                            <div className="searchLabelWrapper">
                                <ControlLabel>All attributes</ControlLabel>
                            </div>
                            <div className="searchSelectWrapper">
                                <FormControl
                                    className="searchSelect"
                                    type="text"
                                    onChange={(event) => this.setState({search: EventHelper.getValue(event)})}
                                    placeholder="Enter search request"
                                />
                            </div>
                        </FormGroup>
                        <Button
                            className="filterSearchButton"
                            bsStyle="primary"
                            onClick={() => this.search()}
                        >
                            SEARCH
                        </Button>
                    </div>
                </div>
                <div hidden={!this.state.isLoading}>
                    <Spinner/>
                </div>
                <div hidden={this.state.isLoading}>
                    <Table responsive className="table">
                        <thead>
                        <tr key="Headers" className="headers">
                            <th>Name</th>
                            <th>Email</th>
                            <th>Office</th>
                            <th>Phone</th>
                            <th>Job Title</th>
                            <th>Manager</th>
                            {attributeNameElements}
                            <th/>
                        </tr>
                        </thead>
                        <tbody>
                        {employeeElements}
                        </tbody>
                    </Table>
                </div>
            </div>
        </div>);
    }

    private getOptions(array: string[]): Array<Option<string>> {
        let arrayOptions: Option<string>[] = array.map(m => {
            return {label: m, value: m}
        });
        arrayOptions.unshift({label: "", value: ""});

        return arrayOptions;
    }

    public componentDidMount(): void {
        this.setState({isLoading: true});
        this.componentDidMountAsync().then();
    }

    private async componentDidMountAsync(): Promise<void> {
        let getAllEmployeesResponse = await this.props.employeeService.getAll(
            this.state.searchManagerSelected,
            this.state.searchOfficeSelected,
            this.state.searchJobTitleSelected,
            this.state.search,
            this.state.searchAttributeValues);

        this.setState({
            employees: getAllEmployeesResponse.employees,
            attributes: getAllEmployeesResponse.attributes,
            searchManagers: getAllEmployeesResponse.managerNames,
            searchOffices: getAllEmployeesResponse.offices,
            searchJobTitles: getAllEmployeesResponse.jobTitles,
            isLoading: false
        });
    }

    private getEmployeeAttributes(employee: Employee): JSX.Element[] {
        return this.state.attributes.map(attributeInfo => {
            let attributeBases = employee.attributes.filter(a => a.attributeInfo.id == attributeInfo.id);

            if (attributeBases.length > 0) {
                let attribute = attributeBases[0];
                if (attributeInfo.type == AttributeType.Document) {
                    console.log(`file Exists: ${attribute.value}`);
                    return <td key={attributeInfo.id}>
                        {
                            attribute.value == "true" ?
                                <img
                                    className="download"
                                    onClick={() => window.location.href = `api/document/download/${employee.login}/${attributeInfo.id}`}
                                    src="/assets/download.png"
                                />
                                : null
                        }
                    </td>;
                }

                return <td key={attributeInfo.id}>{attribute.value}</td>;
            } else {
                return <td key={attributeInfo.id}/>;
            }
        });
    }

    private hideModal(): void {
        this.setState({showModal: false, isLoading: true});
        this.componentDidMountAsync().then();
    }

    private search() {
        this.setState({isLoading: true});

        this.props.employeeService.getAll(
            this.state.searchManagerSelected,
            this.state.searchOfficeSelected,
            this.state.searchJobTitleSelected,
            this.state.search,
            this.state.searchAttributeValues)
            .then((list) => {
                this.setState({employees: list.employees, attributes: list.attributes, isLoading: false});
            });
    }

    private setSearchAttributeValue(attributeInfoId: number, value: string) {
        this.setState(prevState => {
            let currentDict = prevState.searchAttributeValues;
            if (!StringHelper.isNullOrEmpty(value)) {
                currentDict.set(attributeInfoId, value);
            } else {
                currentDict.delete(attributeInfoId);
            }
            return {
                searchAttributeValues: currentDict
            };
        })
    }
}