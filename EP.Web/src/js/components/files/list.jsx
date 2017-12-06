import React, { Component } from "react";
import { render } from 'react-dom';
import { Container, Header, Image, Table, Form, List } from 'semantic-ui-react';

import { squareImage } from '../../../assets/images/image.png';
import API from '../api';

const styles = {
    marginTop: '5em',
    marginLeft: '0.5em'
};

class FileList extends Component {
    constructor(props) {
        super(props);
        this.state = {
            files: []
        }

        API.getNews();
    }

    onUpload = (file) => {
        let reader = new FileReader();
        reader.onloadend = () => {
            this.setState((preState, props) => {
                var arrayvar = preState.files.slice();
                file.url = reader.result;
                arrayvar.push(file);
                return { files: arrayvar };
            });
        }
        reader.readAsDataURL(file);
    }

    render() {
        let fileInput = null;
        const uid = 'testinput';
        const color = 'red';
        return (<Container >
            <Form>
                <Form.Group widths='equal'>
                    <label htmlFor={uid} style={styles} className="ui icon button">
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
                {this.state.files.map(file => (
                    <List.Item key={file.name}>
                        <Image avatar src={file.url} size='small' />
                        <List.Content>
                            {file.name}
                        </List.Content>
                    </List.Item>
                ))}
            </List>
            <Table color={color} key={color}>
                <Table.Header>
                    <Table.Row>
                        <Table.HeaderCell>File name</Table.HeaderCell>
                        <Table.HeaderCell>Preview</Table.HeaderCell>
                    </Table.Row>
                </Table.Header>
                <Table.Body>
                    <Table.Row>
                        <Table.Cell>
                            test.png
                        </Table.Cell>
                        <Table.Cell>
                            <Image avatar src={require('../../../assets/images/image.png')} />
                        </Table.Cell>
                    </Table.Row>
                </Table.Body>
            </Table>
        </Container>);
    };
}

export default FileList;
