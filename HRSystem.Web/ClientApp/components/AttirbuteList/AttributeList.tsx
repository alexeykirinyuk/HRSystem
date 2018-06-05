import * as React from "react";
import { IAttributeListProps } from "./IAttributeListProps";
import { IAttributeListState } from "./IAttributeListState";
import { Button, Table } from "react-bootstrap";
import { SaveAttribute } from "../SaveAttribute/SaveAttribute";
import { AttributeType } from "../../models/AttributeType";
import { AttributeInfo } from "../../models/AttributeInfo";
import { Spinner } from "../Spinner/Spinner";
import "./AttributeList.scss";

export class AttributeList extends React.Component<IAttributeListProps, IAttributeListState> {
    public constructor(props: IAttributeListProps) {
        super(props);

        this.state = {
            attributes: [],
            showModal: false,
            isLoading: true
        };
    }

    public render(): React.ReactElement<IAttributeListProps> {
        return (<div className="attributeList">
            <div className="attributeListInner">
                <SaveAttribute
                    id={this.state.idUpdate}
                    show={this.state.showModal}
                    onHide={() => this.hideModal()}
                    service={this.props.service}/>
                <div className="commandBlock">
                    <div className="commandBlockInner">
                        <Button
                            bsStyle="success"
                            onClick={() => this.setState({showModal: true})}
                        >
                            CREATE NEW
                        </Button>
                    </div>
                </div>
                <div hidden={!this.state.isLoading}>
                    <Spinner />
                </div>
                <div hidden={this.state.isLoading}>
                    <Table responsive>
                        <thead>
                        <tr>
                            <th>Name</th>
                            <th>Type</th>
                            <th/>
                            <th/>
                        </tr>
                        </thead>
                        <tbody>
                        {
                            this.state.attributes.map(a => <tr key={a.id}>
                                <td>{a.name}</td>
                                <td>{AttributeType[a.type]}</td>
                                <td>
                                    <Button
                                        bsStyle="primary"
                                        onClick={() => this.update(a)}>UPDATE</Button>
                                </td>
                                <td>
                                    <Button
                                        bsStyle="danger"
                                        onClick={() => this.delete(a)}>DELETE</Button>
                                </td>
                            </tr>)
                        }
                        </tbody>
                    </Table>
                </div>
            </div>
        </div>);
    }

    public componentDidMount(): void {
        this.componentDidMountAsync().then();
    }

    private async componentDidMountAsync(): Promise<void> {
        let response = await this.props.service.getAll();
        this.setState({attributes: response.attributes, isLoading: false});
    }

    private update(a: AttributeInfo) {
        this.setState({showModal: true, idUpdate: a.id});
    }

    private hideModal(): void {
        this.setState({showModal: false, isLoading: true, idUpdate: null});
        this.componentDidMountAsync().then();
    }

    private delete(a: AttributeInfo) {
        this.setState({showModal: false, isLoading: true, idUpdate: null});
        this.props.service.deleteAttribute(a.id).then(() => {
            this.componentDidMountAsync().then();
        });
    }
}