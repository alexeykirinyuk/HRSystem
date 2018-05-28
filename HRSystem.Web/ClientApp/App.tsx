import {Route, Switch} from "react-router";
import * as React from "react";
import {EmployeeList} from "./components/EmployeeList/EmployeeList";
import {IEmployeeService} from "./core/IEmployeeService";
import {EmployeeService} from "./services/EmployeeService";
import {IDataService} from "./core/IDataService";
import {DataService} from "./services/DataService";
import {Nav, Navbar, NavItem} from "react-bootstrap";
import {Link} from "react-router-dom";
import {AttributeList} from "./components/AttirbuteList/AttributeList";
import {IAttributeService} from "./core/IAttributeService";
import {AttributeService} from "./services/AttributeService";

const baseUrl: string = "http://localhost:5000";
const dataService: IDataService = new DataService(baseUrl);
const employeeService: IEmployeeService = new EmployeeService(dataService);
const attributeService: IAttributeService = new AttributeService(dataService);

export const App = () =>
    <div>
        <Navbar>
            <Navbar.Header>
                <Navbar.Brand>
                    <Link to="/">Employees</Link>
                </Navbar.Brand>
            </Navbar.Header>
            <Nav>
                <NavItem eventKey={1}>
                    <Link to="/attributes">Attributes</Link>
                </NavItem>
            </Nav>
        </Navbar>
        <Switch>
            <Route exact path="/" component={() => <EmployeeList service={employeeService}/>}/>
            <Route exact path="/attributes" component={() => <AttributeList service={attributeService}/>}/>
        </Switch>
    </div>;