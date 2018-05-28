import {IAddEmployeeProps} from "./IAddEmployeeProps";
import {IAddEmployeeState} from "./IAddEmployeeState";
import * as React from "react";
import {Button, ControlLabel, FormControl, FormGroup, Modal} from "react-bootstrap";
import {AttributeType} from "../../models/AttributeType";
import {AttributeInfo} from "../../models/AttributeInfo";
import {StringHelper} from "../../helpers/StringHelper";
import {EventHelper} from "../../helpers/EventHelper";
import Select, {Option} from "react-select";
import {Employee} from "../../models/Employee";

export class AddEmployee extends React.Component<IAddEmployeeProps, IAddEmployeeState> {
    private static readonly typeMap: Map<AttributeType, string> =
        new Map<AttributeType, string>();

    static _initialize(): void {
        this.typeMap.set(AttributeType.Int, "number");
        this.typeMap.set(AttributeType.String, "text");
        this.typeMap.set(AttributeType.DateTime, "date");
    }

    public constructor(props: IAddEmployeeProps) {
        super(props);

        this.state = {
            attributes: [],
            firstName: StringHelper.Empty,
            email: StringHelper.Empty,
            jobTitle: StringHelper.Empty,
            lastName: StringHelper.Empty,
            login: StringHelper.Empty,
            phone: StringHelper.Empty,
            attributesInfo: [],
            isLoading: true,
            employees: [],
            show: props.show,
            onHide: props.onHide,
            managerOption: null,
            managerLogin: null
        };
    }

    public render(): React.ReactElement<IAddEmployeeProps> {
        let mapToSelect = (e: Employee) => {
            return {
                value: e,
                label: e.fullName
            }
        };

        return (
            <Modal show={this.state.show} onHide={() => this.onHide()}>
                <Modal.Header>Create new employee</Modal.Header>
                <Modal.Body>
                    <form>
                        <FormGroup>
                            <ControlLabel>Login</ControlLabel>
                            <FormControl
                                type="text"
                                placeholder="Enter login"
                                onChange={(event) => this.setState({login: EventHelper.getValue(event)})}/>
                        </FormGroup>
                        <FormGroup>
                            <ControlLabel>First name</ControlLabel>
                            <FormControl
                                type="text"
                                placeholder="Enter first name"
                                onChange={(event) => this.setState({firstName: EventHelper.getValue(event)})}/>
                        </FormGroup>
                        <FormGroup>
                            <ControlLabel>Last name</ControlLabel>
                            <FormControl
                                type="text"
                                placeholder="Enter last name"
                                onChange={(event) => this.setState({lastName: EventHelper.getValue(event)})}/>
                        </FormGroup>
                        <FormGroup>
                            <ControlLabel>Email</ControlLabel>
                            <FormControl
                                type="text"
                                placeholder="Enter email address"
                                onChange={(event) => this.setState({email: EventHelper.getValue(event)})}/>
                        </FormGroup>
                        <FormGroup>
                            <ControlLabel>Manager</ControlLabel>
                            <Select
                                value={this.state.managerOption}
                                onChange={option => this.setState({
                                    managerLogin: (option as Option<Employee>).value.login,
                                    managerOption: option
                                })}
                                options={this.state.employees.map(mapToSelect)}>
                            </Select>
                        </FormGroup>
                        <FormGroup>
                            <ControlLabel>Job Title</ControlLabel>
                            <FormControl
                                type="text"
                                placeholder="Enter job title"
                                onChange={(event) => this.setState({jobTitle: EventHelper.getValue(event)})}/>
                        </FormGroup>
                        <FormGroup>
                            <ControlLabel>Phone</ControlLabel>
                            <FormControl
                                type="text"
                                placeholder="Enter phone"
                                onChange={(event) => this.setState({phone: EventHelper.getValue(event)})}/>
                        </FormGroup>
                        {
                            this.state.attributesInfo.map(a => <FormGroup>
                                <ControlLabel>{a.name}</ControlLabel>
                                <FormControl
                                    type={AddEmployee.typeMap.get(a.type)}
                                    placeholder={`Enter ${a.name}`}
                                    onChange={(event) => this.changeAttribute(a, event)}/>
                            </FormGroup>)
                        }
                    </form>
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={() => this.setState({show: false})}>Close</Button>
                    <Button bsStyle="primary" onClick={() => this.clickSaveButton()}>Create</Button>
                </Modal.Footer>
            </Modal>);
    }

    public componentWillMount(): void {
        this.setState({isLoading: true});
        this.componentWillMountAsync().then();
    }

    private async componentWillMountAsync(): Promise<void> {
        let info = await this.props.service.getEmployeeCreationInfo();
        console.log(info);
        this.setState({
            attributesInfo: info.attributes,
            isLoading: false,
            employees: info.employees
        });
    }

    private changeAttribute(attributeInfo: AttributeInfo, event: any): void {
        this.setState(state => {
            if (event.target != null) {
                let resultAttributes = state.attributes.slice();
                let filteredById = resultAttributes.filter(a => a.attributeInfoId == attributeInfo.id);
                if (filteredById.length == 0) {
                    resultAttributes.push({
                        attributeInfoId: attributeInfo.id,
                        type: attributeInfo.type,
                        value: event.target.value
                    });
                } else {
                    filteredById[0].value = event.target.value;
                }

                return {attributes: resultAttributes};
            }
        });
    }

    private clickSaveButton(): void {
        this.clickSaveButtonAsync().then();
    }

    componentWillReceiveProps(props: IAddEmployeeProps): void {
        this.setState({show: props.show, onHide: props.onHide});
    }

    private async clickSaveButtonAsync(): Promise<void> {
        try {
            await this.props.service.addNewEmployee(this.state);
            this.setState({show: false});
        } catch (e) {
            console.log(e);
        }
    }

    private onHide() {
        if (this.state.onHide != null) {
            this.state.onHide();
        }
    }
}