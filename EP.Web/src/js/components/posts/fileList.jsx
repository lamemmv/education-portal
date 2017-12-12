import React, { Component } from "react";
import { render } from 'react-dom';
import { Link } from 'react-router-dom';

import { Container, Image, Table, Menu, Icon, Button, Modal } from 'semantic-ui-react';
import Upload from '../files/upload';
import Pagination from './pagination/index';
import PageSetting from '../../settings/page';

class FileList extends Component {
    componentWillMount() {
        this.props.getFiles(this.props.fileState.currentPage);
    }

    renderTableFooter(pages, currentPage, showPagination) {
        return (
            showPagination ? (
                <Pagination/>
            ) : null
        );
    }

    renderTableBody(files) {
        return (
            files.map((file) => {
                return (<Table.Row key={file.id}>
                    <Table.Cell>
                        {file.fileName}
                    </Table.Cell>
                    <Table.Cell>
                        <Link to={`/files/${file.id}`}>
                            <Image avatar src={require('../../../assets/images/image.png')} />
                        </Link>
                    </Table.Cell>
                    <Table.Cell>
                        {/* <Button onClick={() => this.askForDeleting(file)}>Delete</Button> */}
                        <Button >Delete</Button>
                    </Table.Cell>
                </Table.Row>)
            })
        );
    }

    render() {
        const {
            showDeleteConfirmation,
            files,
            currentPage,
            pages,
            fileToBeDeleted,
            showPagination
        } = this.props.fileState;
        const color = 'red';
        return (<Container >
            {/* <Upload onUploaded={this.onUploaded} /> */}
            <Table color={color} key={color}>
                <Table.Header>
                    <Table.Row>
                        <Table.HeaderCell>File name</Table.HeaderCell>
                        <Table.HeaderCell>Preview</Table.HeaderCell>
                        <Table.HeaderCell></Table.HeaderCell>
                    </Table.Row>
                </Table.Header>
                <Table.Body>
                    {this.renderTableBody(files)}
                </Table.Body>
                {this.renderTableFooter(pages, currentPage, showPagination)}
            </Table>
            <Modal
                open={showDeleteConfirmation}
                closeOnEscape={false}
                closeOnRootNodeClick={false}
                onClose={this.close}>
                <Modal.Header>
                    Delete File
                </Modal.Header>
                <Modal.Content>
                    <p>Are you sure you want to delete file</p>
                </Modal.Content>
                <Modal.Actions>
                    {/* <Button negative onClick={this.closeModal}>No</Button> */}
                    <Button negative>No</Button>
                    {/* <Button positive
                        labelPosition='right'
                        icon='checkmark'
                        content='Yes' onClick={this.confirmDeleteFile} /> */}
                    <Button positive
                        labelPosition='right'
                        icon='checkmark'
                        content='Yes' />
                </Modal.Actions>
            </Modal>
        </Container>);
    };
}

export default FileList;
