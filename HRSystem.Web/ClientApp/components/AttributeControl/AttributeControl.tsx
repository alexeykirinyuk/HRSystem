import { IAttributeControlState } from "./IAttributeControlState";
import { IAttributeControlProps } from "./IAttributeControlProps";
import * as React from "react";
import { SaveEmployee } from "../SaveEmployee/SaveEmployee";
import { AttributeType } from "../../models/AttributeType";
import { EventHelper } from "../../helpers/EventHelper";
import {Button, Checkbox, ControlLabel, FormControl, FormGroup} from "react-bootstrap";
import { StringHelper } from "../../helpers/StringHelper";
import { FormEventHandler } from "react";

export class AttributeControl extends React.Component<IAttributeControlProps, IAttributeControlState> {
    private static readonly typeMap: Map<AttributeType, string> =
        new Map<AttributeType, string>();

    static _initialize(): void {
        this.typeMap.set(AttributeType.Int, "number");
        this.typeMap.set(AttributeType.String, "text");
        this.typeMap.set(AttributeType.DateTime, "date");
        this.typeMap.set(AttributeType.Bool, "checkbox");
    }

    public constructor(props: IAttributeControlProps) {
        super(props);

        this.state = {};
    }

    public render(): React.ReactElement<IAttributeControlProps> {
        switch (this.props.info.type) {
            case AttributeType.Bool:
                return (<Checkbox
                    key={this.props.info.id}
                    checked={this.props.value == "true"}
                    onChange={(event) => this.change(EventHelper.getBoolValue(event))}/>);
            case AttributeType.Document:
                return (<div>
                    {
                        this.props.value == "true" ?
                            <div>
                                <Button bsStyle="success" onClick={() => this.downloadFile()}>DOWNLOAD</Button>
                                <Button bsStyle="danger" onClick={() => this.deleteFile()}>DELETE</Button></div> :
                            <input type="file" onChange={(a) => this.uploadFile(a)}/>
                    }
                </div>);
            default:
                return (<FormControl
                    key={this.props.info.id}
                    type={AttributeControl.typeMap.get(this.props.info.type)}
                    placeholder={`Enter ${this.props.info.name} (custom)`}
                    value={this.props.value}
                    onChange={(event) => this.change(EventHelper.getValue(event))}/>);
        }
    }

    componentWillReceiveProps(props: IAttributeControlProps) {
        this.setState(props);
    }

    private change(value: string) {
        console.log("change attribute control value: " + value);
        console.log(event);
        this.setState({value: value});
        this.props.onChange(this.props.info, value);
    }

    private uploadFile(event: any) {
        let file = event.target.files[0];
        this.props.onChange(this.props.info, file);
    }

    private deleteFile() {
        if (this.props.onDeleteFile != null) {
            this.props.onDeleteFile(this.props.info);
        }
    }

    private downloadFile() {
        if (this.props.onDownloadFile != null) {
            this.props.onDownloadFile(this.props.info);
        }
    }
}

AttributeControl._initialize();