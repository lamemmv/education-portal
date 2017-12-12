import React, { Component } from "react";
import { render } from 'react-dom';
import { Link } from 'react-router-dom';

import { Container, Image, Table, Button } from 'semantic-ui-react';
import UploadIndex from './upload/index';
import Pagination from './pagination/index';
import DeleteFile from './delete/index';

class FileList extends Component {
    componentWillMount() {
        this.props.getFiles(this.props.fileState.currentPage);
    }

    renderTableFooter(pages, currentPage, showPagination) {
        return (
            showPagination ? (
                <Pagination />
            ) : null
        );
    }

    renderTableBody(files) {
        const { askForDeleting } = this.props;
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
                        <Button onClick={() => askForDeleting(file.id)}>Delete</Button>
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
        const { closeModal } = this.props;
        return (<Container >
            <UploadIndex />
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
            <DeleteFile/>
        </Container>);
    };
}

export default FileList;
