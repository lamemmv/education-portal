import React, { Component } from "react";
import { render } from 'react-dom';
import { Container, Header, Image, Table, Form, List } from 'semantic-ui-react';

import { squareImage } from '../../../assets/keen.png';

const styles = {
    marginTop: '5em'
};

class FileList extends Component {
    constructor(props, context) {
        super(props, context);
    }

    files = [];
    onUpload = (file) => {
        this.files.push(file);
        console.log(file);
        console.log(this.files.length);
    }

    render() {
        let fileInput = null;
        const uid = 'testinput';
        const color = 'red';
        return (<Container style={styles}>
            <Form>
                <Form.Group widths='equal'>
                    <label htmlFor={uid} className="ui icon button">
                        <i className="upload icon"></i>
                        Upload
                </label>
                    <input type="file" id={uid}
                        style={{ display: "none" }}
                        onChange={() => {
                            this.onUpload(fileInput.files[0]);
                        }}
                        ref={input => {
                            fileInput = input;
                        }}
                    />
                </Form.Group>
            </Form>
            <List>
                {this.files.map(file => (
                    <List.Item key={file.name}>
                        <List.Content>{file.name}</List.Content>
                    </List.Item>
                ))}
            </List>
            <Table color={color} key={color}>
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
                                <Image src={squareImage} />
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
