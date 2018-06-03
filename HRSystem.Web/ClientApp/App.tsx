import { Route, Switch } from "react-router";
import * as React from "react";
import { EmployeeList } from "./components/EmployeeList/EmployeeList";
import { IEmployeeService } from "./core/IEmployeeService";
import { EmployeeService } from "./services/EmployeeService";
import { IDataService } from "./core/IDataService";
import { DataService } from "./services/DataService";
import { Nav, Navbar, NavItem } from "react-bootstrap";
import { Link } from "react-router-dom";
import { AttributeList } from "./components/AttirbuteList/AttributeList";
import { IAttributeService } from "./core/IAttributeService";
import { AttributeService } from "./services/AttributeService";
import {LinkContainer} from "react-router-bootstrap";

const baseUrl: string = "http://localhost:5000";
const dataService: IDataService = new DataService(baseUrl);
const employeeService: IEmployeeService = new EmployeeService(dataService);
const attributeService: IAttributeService = new AttributeService(dataService);

export const App = () =>
    <div>
        <Navbar>
            <Navbar.Header>
                <Navbar.Brand>
                    <Link to="/">HR SYSTEM</Link>
                </Navbar.Brand>
            </Navbar.Header>
            <Nav>
                <LinkContainer to="/list">
                    <NavItem eventKey={1}>
                        Employee List
                    </NavItem>
                </LinkContainer>
                <LinkContainer to="/attributes">
                    <NavItem eventKey={2}>
                        Attribute List
                    </NavItem>
                </LinkContainer>
            </Nav>
        </Navbar>
        <Switch>
            <Route exact path="/" component={() => <EmployeeList employeeService={employeeService}/>}/>
            <Route exact path="/list" component={() => <EmployeeList employeeService={employeeService}/>}/>
            <Route exact path="/attributes" component={() => <AttributeList service={attributeService}/>}/>
        </Switch>
    </div>;