import React, { Component } from "react";
import { render } from 'react-dom';
import { Container, Header, Image, Table, Form } from 'semantic-ui-react';

import { squareImage } from '../../../assets/keen.png';

class FileList extends Component {
    constructor(props, context) {
        super(props, context);
    }

    render() {
        let fileInput = null;
        const uid = 'testinput';
        return (<Container>
            <Form>
                <Form.Group widths='equal'>
                    <label htmlFor={uid} className="ui icon button">
                        <i className="upload icon"></i>
                        Upload
                </label>
                    <input type="file" id={uid}
                        style={{ display: "none" }}
                        onChange={() => {
                            onUpload(fileInput.files[0]);
                        }}
                        ref={input => {
                            fileInput = input;
                        }}
                    />
                </Form.Group>
            </Form>
            <Table basic='very' celled collapsing>
                <Table.Header>
                    <Table.Row>
                        <Table.HeaderCell>Employee</Table.HeaderCell>
                        <Table.HeaderCell>Correct Guesses</Table.HeaderCell>
                    </Table.Row>
                </Table.Header>
                <Table.Body>
                    <Table.Row>
                        <Table.Cell>
                            <Header as='h4' image>
                                <Image src={squareImage} rounded size='mini' />
                                <Header.Content>
                                    Lena
                                    <Header.Subheader>Human Resources</Header.Subheader>
                                </Header.Content>
                            </Header>
                        </Table.Cell>
                        <Table.Cell>
                            22
                        </Table.Cell>
                    </Table.Row>
                </Table.Body>
            </Table>

        </Container>
        );
    };
}

export default FileList;
