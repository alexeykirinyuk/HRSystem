import * as React from "react";
import * as ReactDOM from "react-dom";
import {BrowserRouter} from "react-router-dom";
import {App} from "./App";
import {bootstrapUtils} from "react-bootstrap/lib/utils"
import {Button} from "react-bootstrap";

bootstrapUtils.addStyle(Button, "custom");

ReactDOM.render(
    <BrowserRouter>
        <App />
    </BrowserRouter>,
    document.getElementById("example")
);