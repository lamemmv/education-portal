import React, { Component } from "react";
import { render } from 'react-dom';
import { Modal, Button } from 'semantic-ui-react';

class DeleteModal extends Component {

    render() {
        const {
            showDeleteConfirmation,
            fileTobeDeleted
        } = this.props.deleteState;
        const { closeModal, confirmDeleteFile } = this.props;

        return (
            <Modal
                open={showDeleteConfirmation}
                closeOnEscape={false}
                closeOnRootNodeClick={false}>
                <Modal.Header>
                    Delete File
                </Modal.Header>
                <Modal.Content>
                    <p>Are you sure you want to delete file</p>
                </Modal.Content>
                <Modal.Actions>
                    <Button negative onClick={closeModal}>No</Button>
                    <Button positive
                        labelPosition='right'
                        icon='checkmark'
                        content='Yes' onClick={() => confirmDeleteFile(fileTobeDeleted)} />
                </Modal.Actions>
            </Modal>
        );
    }
}

export default DeleteModal;