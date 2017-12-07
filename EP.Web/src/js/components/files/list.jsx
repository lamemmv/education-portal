import React, { Component } from "react";
import { render } from 'react-dom';
import { Link } from 'react-router-dom';

import { Container, Image, Table } from 'semantic-ui-react';
import Upload from './upload';
import API from '../api';

class FileList extends Component {
    constructor(props) {
        super(props);
        this.state = {
            files: []
        }

        this.getFiles();
    }

    getFiles() {
        let url = API.getBaseUri() + 'admin/blobManager';
        fetch(url, {
            method: 'get',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
        })
            .then(response => { return response.json(); })
            .then(response => {
                this.setState((preState, props) => {
                    return { files: response.items };
                });
            })
            .catch(error => { console.log('request failed', error); });
    }

    render() {
        let fileInput = null;
        const uid = 'testinput';
        const color = 'red';
        return (<Container >
            <Upload />
            <Table color={color} key={color}>
                <Table.Header>
                    <Table.Row>
                        <Table.HeaderCell>File name</Table.HeaderCell>
                        <Table.HeaderCell>Preview</Table.HeaderCell>
                    </Table.Row>
                </Table.Header>
                <Table.Body>
                    {this.state.files.map(file => (
                        <Table.Row key={file.id}>
                            <Table.Cell>
                                {file.fileName}
                            </Table.Cell>
                            <Table.Cell>
                                <Link to={'/api/admin/blobManager/${file.id}'}>
                                    <Image avatar src={require('../../../assets/images/image.png')} />
                                </Link>
                            </Table.Cell>
                        </Table.Row>
                    ))}
                </Table.Body>
            </Table>
        </Container>);
    };
}

export default FileList;
