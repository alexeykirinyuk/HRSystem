import {ISaveEmployeeProps} from "./ISaveEmployeeProps";
import {ISaveEmployeeState} from "./ISaveEmployeeState";
import * as React from "react";
import {Alert, Button, ControlLabel, FormControl, FormGroup, Modal} from "react-bootstrap";
import {AttributeInfo} from "../../models/AttributeInfo";
import {StringHelper} from "../../helpers/StringHelper";
import {EventHelper} from "../../helpers/EventHelper";
import Select, {Option} from "react-select";
import {ValidationErrors} from "../../models/ValidationErrors";
import {AttributeControl} from "../AttributeControl/AttributeControl";

export class SaveEmployee extends React.Component<ISaveEmployeeProps, ISaveEmployeeState> {

    public constructor(props: ISaveEmployeeProps) {
        super(props);

        this.state = this.getEmptyState(props.show, props.isCreate);
    }

    public render(): React.ReactElement<ISaveEmployeeProps> {
        let managerOptions: Option<string>[] = this.state.employees.map(e => {
            return {
                label: e.fullName,
                value: e.login
            }
        });
        managerOptions.unshift({label: "", value: ""});

        return (
            <Modal show={this.state.show} onHide={() => this.hide()}
                   onEscapeKeyUp={() => this.hide()}>
                <Modal.Header>Create new employee</Modal.Header>
                <Modal.Body>
                    <div hidden={!this.state.isLoading}>
                        Loading...
                    </div>
                    <form hidden={this.state.isLoading}>
                        <Alert hidden={this.state.validationErrors.length == 0}>
                            <ul>
                                {
                                    this.state.validationErrors.map(e => <li key={e.message}>{e.message}</li>)
                                }
                            </ul>
                        </Alert>
                        <FormGroup>
                            <ControlLabel>Login</ControlLabel>
                            <FormControl
                                type="text"
                                value={this.state.login}
                                disabled={!this.state.isCreateCommand}
                                placeholder="Enter login"
                                onChange={(event) => this.setState({login: EventHelper.getValue(event)})}/>
                        </FormGroup>
                        <FormGroup>
                            <ControlLabel>First name</ControlLabel>
                            <FormControl
                                type="text"
                                value={this.state.firstName}
                                placeholder="Enter first name"
                                onChange={(event) => this.setState({firstName: EventHelper.getValue(event)})}/>
                        </FormGroup>
                        <FormGroup>
                            <ControlLabel>Last name</ControlLabel>
                            <FormControl
                                type="text"
                                value={this.state.lastName}
                                placeholder="Enter last name"
                                onChange={(event) => this.setState({lastName: EventHelper.getValue(event)})}/>
                        </FormGroup>
                        <FormGroup>
                            <ControlLabel>Email</ControlLabel>
                            <FormControl
                                type="text"
                                value={this.state.email}
                                placeholder="Enter email address"
                                onChange={(event) => this.setState({email: EventHelper.getValue(event)})}/>
                        </FormGroup>
                        <FormGroup>
                            <ControlLabel>Manager</ControlLabel>
                            <Select
                                value={this.state.managerLoginOption}
                                onChange={option => this.setState({
                                    managerLogin: option == null ? managerOptions[0].value : (option as Option<string>).value,
                                    managerLoginOption: option == null ? managerOptions[0] : option
                                })}
                                options={managerOptions}>
                            </Select>
                        </FormGroup>
                        <FormGroup>
                            <ControlLabel>Job Title</ControlLabel>
                            <FormControl
                                type="text"
                                placeholder="Enter job title"
                                value={this.state.jobTitle}
                                onChange={(event) => this.setState({jobTitle: EventHelper.getValue(event)})}/>
                        </FormGroup>
                        <FormGroup>
                            <ControlLabel>Office</ControlLabel>
                            <FormControl
                                type="text"
                                placeholder="Enter office"
                                value={this.state.office}
                                onChange={(event) => this.setState({office: EventHelper.getValue(event)})}/>
                        </FormGroup>
                        <FormGroup>
                            <ControlLabel>Phone</ControlLabel>
                            <FormControl
                                type="text"
                                placeholder="Enter phone"
                                value={this.state.phone}
                                onChange={(event) => this.setState({phone: EventHelper.getValue(event)})}/>
                        </FormGroup>
                        {
                            this.state.attributesInfo.map(a => <FormGroup key={a.id}>
                                <ControlLabel>{a.name}</ControlLabel>
                                <AttributeControl
                                    info={a}
                                    placeholder={`Enter ${a.name} (custom)`}
                                    value={this.getAttribute(a)}
                                    onChange={(a: AttributeInfo, value: string) => this.changeAttribute(a, value)}
                                    onDeleteFile={(a: AttributeInfo) => this.deleteDocument(a)}
                                onDownloadFile={(a: AttributeInfo) => this.downloadDocument(a)}/>
                            </FormGroup>)
                        }
                    </form>
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={() => this.hide()}>Close</Button>
                    <Button bsStyle="primary"
                            onClick={() => this.clickSaveButton()}>{this.state.isCreateCommand ? "Create" : "Update"}</Button>
                </Modal.Footer>
            </Modal>);
    }

    private getAttribute(a: AttributeInfo): any {
        let result = this.state.attributes.filter(at => at.attributeInfoId == a.id);
        if (result.length != 0) {
            return result[0].value;
        }

        return StringHelper.EMPTY;
    }

    public componentWillReceiveProps(props: ISaveEmployeeProps): void {
        if (props.show && !this.state.show) {
            this.componentWillMountAsync(props.login, props.isCreate).then();
        }
        this.setState({show: props.show, isCreateCommand: props.isCreate});
    }

    private async componentWillMountAsync(login: string, isCreate: boolean): Promise<void> {
        this.setState({isLoading: true});
        let info = await this.props.employeeService.getEmployeeSavingInfo(login, isCreate);
        console.log(info.employee.managerLogin);
        console.log(info.employee.managerName);

        this.setState({
            attributesInfo: info.attributes,
            isLoading: false,
            employees: info.employees,

            login: info.employee.login,
            firstName: info.employee.firstName,
            lastName: info.employee.lastName,
            email: info.employee.email,
            phone: info.employee.phone,
            jobTitle: info.employee.jobTitle,
            office: info.employee.office,
            managerLogin: info.employee.managerLogin,
            managerLoginOption: {label: info.employee.managerName, value: info.employee.managerLogin},
            attributes: info.employee.attributes.map(a => {
                return {attributeInfoId: a.attributeInfo.id, type: a.attributeInfo.type, value: a.value}
            })
        });
    }

    private changeAttribute(attributeInfo: AttributeInfo, value: string): void {
        console.log(`Update ${attributeInfo.name}: ${value}`);
        this.setState(state => {
            let resultAttributes = state.attributes.slice();
            let filteredById = resultAttributes.filter(a => a.attributeInfoId == attributeInfo.id);
            if (filteredById.length == 0) {
                resultAttributes.push({
                    attributeInfoId: attributeInfo.id,
                    type: attributeInfo.type,
                    value: value
                });
            } else {
                filteredById[0].value = value;
            }

            return {attributes: resultAttributes};
        });
    }

    private clickSaveButton(): void {
        this.clickSaveButtonAsync().then();
    }

    private async clickSaveButtonAsync(): Promise<void> {
        try {
            await this.props.employeeService.save(this.state);
            this.hide();
        } catch (e) {
            if (ValidationErrors.isValidationError(e)) {
                this.setState({validationErrors: ValidationErrors.parse(e)});
            }

            console.log(e);
        }
    }

    private hide() {
        if (this.props.onHide != null) {
            this.props.onHide();
        }
        this.setState(this.getEmptyState(false, false));
    }

    private getEmptyState(show: boolean, isCreate: boolean): ISaveEmployeeState {
        return {
            attributes: [],
            firstName: StringHelper.EMPTY,
            email: StringHelper.EMPTY,
            jobTitle: StringHelper.EMPTY,
            lastName: StringHelper.EMPTY,
            login: StringHelper.EMPTY,
            phone: StringHelper.EMPTY,
            attributesInfo: [],
            isLoading: true,
            employees: [],
            show: show,
            managerLoginOption: null,
            managerLogin: null,
            isCreateCommand: isCreate,
            office: StringHelper.EMPTY,
            validationErrors: []
        };
    }

    private deleteDocument(attributeInfo: AttributeInfo) {
        this.setState({isLoading: true});
        this.props.employeeService.deleteDocument(this.state.login, attributeInfo.id).then(() => {
            this.setState(prevState => {
                return {
                    attributes: prevState.attributes.filter(a => a.attributeInfoId != attributeInfo.id),
                    isLoading: false
                };
            });
        });
    }

    private downloadDocument(a: AttributeInfo) {
        window.open(`api/document/download/${this.state.login}/${a.id}`, "_blank    ");
    }
}