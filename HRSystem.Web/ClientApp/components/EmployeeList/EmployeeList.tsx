import {IEmployeeListProps} from "./IEmployeeListProps";
import {IEmployeeListState} from "./IEmployeeListState";
import * as React from "react";
import {Button, ControlLabel, FormControl, FormGroup, Image, Table} from "react-bootstrap/lib";
import {Spinner} from "../Spinner/Spinner";
import {SaveEmployee} from "../SaveEmployee/SaveEmployee";
import {StringHelper} from "../../helpers/StringHelper";
import {Employee} from "../../models/Employee";
import {AttributeType} from "../../models/AttributeType";
import {EventHelper} from "../../helpers/EventHelper";

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
            search: StringHelper.EMPTY
        };
    }

    public render(): React.ReactElement<IEmployeeListProps> {
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

        return (<div>
            <SaveEmployee
                employeeService={this.props.employeeService}
                show={this.state.showModal}
                onHide={() => this.hideModal()}
                isCreate={this.state.isCreateModalType}
                login={this.state.selectedEmployeeLogin}/>
            <div>
                <Button bsStyle="success" onClick={() => this.setState({showModal: true, isCreateModalType: true})}>
                    CREATE</Button>
            </div>
            <div>
                <FormGroup>
                    <FormControl
                        width={"50%"}
                        type="text"
                        onChange={(event) => this.setState({search: EventHelper.getValue(event)})}
                        placeholder="Enter search request"/>
                    <Button bsStyle="primary" onClick={() => this.search()}>
                        SEARCH</Button>
                </FormGroup>
            </div>
            <div hidden={!this.state.isLoading}>
                <Spinner/>
            </div>
            <div hidden={this.state.isLoading}>
                <Table striped bordered condensed hover>
                    <thead>
                    <tr key="Headers">
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
        </div>);
    }

    public componentDidMount(): void {
        this.setState({isLoading: true});
        this.componentDidMountAsync().then();
    }

    private async componentDidMountAsync(): Promise<void> {
        let getAllEmployeesResponse = await this.props.employeeService.getAll(  );
        this.setState({
            employees: getAllEmployeesResponse.employees,
            attributes: getAllEmployeesResponse.attributes,
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
                                <Button
                                    bsStyle="link"
                                    href={`api/document/download/${employee.login}/${attributeInfo.id}`}
                                    target={"_blank"}>
                                    DOWNLOAD
                                </Button> : null
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
        this.props.employeeService.getAll(this.state.search).then((list) => {
            this.setState({employees: list.employees, attributes: list.attributes, isLoading: false});
        });
    }
}