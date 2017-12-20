import { h, Component } from "preact";
import { Link } from 'react-router-dom';

// import { Container, Image, Table, Menu, Icon, Button, Modal } from 'semantic-ui-react';
import Upload from './upload';
import API from '../api';
import PageSetting from '../../settings/page';

class FileList extends Component {
    constructor(props, context) {
        super(props, context);
        this.state = {
            files: [],
            showDeleteConfirmation: false,
            fileTobeDeleted: null,
            showPagination: false,
            pages: [],
            currentPage: 1
        }
        this.getFiles = this.getFiles.bind(this);
        this.askForDeleting = this.askForDeleting.bind(this);
        this.onUploaded = this.onUploaded.bind(this);
        this.confirmDeleteFile = this.confirmDeleteFile.bind(this);
        this.closeModal = this.closeModal.bind(this);
        this.calculatePagination = this.calculatePagination.bind(this);
        this.getFiles(1);
    }

    getFiles(currentPage) {
        let request = { page: currentPage ? currentPage : 1, size: PageSetting.getPageSize() };
        let url = API.getBaseUri() + 'admin/blobManager';
        url += (url.indexOf('?') === -1 ? '?' : '&') + API.queryParams(request);
        fetch(url, {
            method: 'get',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            }
        })
            .then(response => { return response.json(); })
            .then(response => {
                if (response.items != null) {
                    this.setState((preState, props) => {
                        return { files: response.items, currentPage: currentPage ? currentPage : 1 };
                    });
                }

                this.calculatePagination(response);
            })
            .catch(error => { console.log('request failed', error); });
    }

    askForDeleting(file) {
        this.setState((prevState, props) => {
            return { showDeleteConfirmation: true, fileTobeDeleted: file };
        });
    }

    closeModal() {
        this.setState((prevState, props) => {
            return { showDeleteConfirmation: false };
        });
    }

    confirmDeleteFile() {
        this.closeModal();
        let url = API.getBaseUri() + 'admin/blobManager?id=' + this.state.fileTobeDeleted.id;
        fetch(url, {
            method: 'delete',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            }
        })
            .then(response => {
                this.getFiles();
            })
            .catch(error => { console.log('request failed', error); });
    }

    onUploaded() {
        this.getFiles();
    }

    calculatePagination(response) {
        if (response.totalPages > 1) {

            var pages = [];
            for (let i = 0; i < response.totalPages; i++) {
                pages.push(i + 1);
            }

            this.setState((prevState, props) => {
                return {
                    showPagination: true,
                    pages: pages
                };
            });
        }
    }

    render() {
        const { showDeleteConfirmation } = this.state;
        const color = 'red';
        return (
        // <Container >
        //     <Upload onUploaded={this.onUploaded} />
        //     <Table color={color} key={color}>
        //         <Table.Header>
        //             <Table.Row>
        //                 <Table.HeaderCell>File name</Table.HeaderCell>
        //                 <Table.HeaderCell>Preview</Table.HeaderCell>
        //                 <Table.HeaderCell></Table.HeaderCell>
        //             </Table.Row>
        //         </Table.Header>
        //         <Table.Body>
        //             {this.state.files.map(file => (
        //                 <Table.Row key={file.id}>
        //                     <Table.Cell>
        //                         {file.fileName}
        //                     </Table.Cell>
        //                     <Table.Cell>
        //                         <Link to={`/files/${file.id}`}>
        //                             <Image avatar src={require('../../../assets/images/image.png')} />
        //                         </Link>
        //                     </Table.Cell>
        //                     <Table.Cell>
        //                         <Button onClick={() => this.askForDeleting(file)}>Delete</Button>
        //                     </Table.Cell>
        //                 </Table.Row>
        //             ))}
        //         </Table.Body>
        //         {
        //             this.state.showPagination ? (
        //                 <Table.Footer>
        //                     <Table.Row>
        //                         <Table.HeaderCell colSpan='3'>
        //                             <Menu floated='right' pagination>
        //                                 <Menu.Item as='a' icon
        //                                     onClick={() => this.getFiles(this.state.currentPage - 1)}
        //                                     disabled={this.state.currentPage <= 1}>
        //                                     <Icon name='left chevron' />
        //                                 </Menu.Item>
        //                                 {
        //                                     this.state.pages.map(page => (
        //                                         <Menu.Item as='a' key={page} onClick={() => this.getFiles(page)}
        //                                             active={page == this.state.currentPage}>{page}
        //                                         </Menu.Item>
        //                                     ))
        //                                 }
        //                                 <Menu.Item as='a' icon
        //                                     onClick={() => this.getFiles(this.state.currentPage + 1)}
        //                                     disabled={this.state.currentPage >= this.state.pages.length}>
        //                                     <Icon name='right chevron' />
        //                                 </Menu.Item>
        //                             </Menu>
        //                         </Table.HeaderCell>
        //                     </Table.Row>
        //                 </Table.Footer>
        //             ) : null
        //         }
        //     </Table>
        //     <Modal
        //         open={showDeleteConfirmation}
        //         closeOnEscape={false}
        //         closeOnRootNodeClick={false}
        //         onClose={this.close}>
        //         <Modal.Header>
        //             Delete File
        //         </Modal.Header>
        //         <Modal.Content>
        //             <p>Are you sure you want to delete file</p>
        //         </Modal.Content>
        //         <Modal.Actions>
        //             <Button negative onClick={this.closeModal}>No</Button>
        //             <Button positive
        //                 labelPosition='right'
        //                 icon='checkmark'
        //                 content='Yes' onClick={this.confirmDeleteFile} />
        //         </Modal.Actions>
        //     </Modal>
        // </Container>
        <div>file list</div>
        );
    };
}

export default FileList;
