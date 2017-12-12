import React, { Component } from "react";
import { render } from 'react-dom';
import { Container, Button, Image, Form, List } from 'semantic-ui-react';

const styles = {
    marginTop: '5em',
    marginLeft: '0.5em'
};

class Upload extends Component {

    selectFile(file) {
        let reader = new FileReader();
        reader.onloadend = () => {
            file.url = reader.result;            
            this.props.selectFile(file);
        }
        reader.readAsDataURL(file);
    }

    render() {
        let fileInput = null;
        const uid = 'testinput';
        const { removeItem, selectFile, uploadFile } = this.props;
        const { files } = this.props.uploadState;
        const supportedImages = ['png', 'jpg', 'gif', 'jpeg'];
        return (<Container >
            <Form>
                <Form.Group>
                    <label htmlFor={uid} style={styles} className="ui icon button">
                        <i className="upload icon"></i>
                        Select a file
                    </label>
                    <input type="file" id={uid}
                        style={{ display: "none" }}
                        onChange={() => {
                            this.selectFile(fileInput.files[0]);
                        }}
                        ref={input => {
                            fileInput = input;
                        }}
                    />
                    <Form.Button
                        onClick={() => uploadFile(files)}
                        style={styles}
                        disabled={files.length == 0}>Upload all</Form.Button>
                </Form.Group>
            </Form>
            <List divided verticalAlign='middle'>
                {files.map(file => (
                    <List.Item key={file.name}>
                        <List.Content floated='right'>
                            <Button onClick={() => removeItem(file)}>Remove</Button>
                        </List.Content>
                        {supportedImages.indexOf(file.name.substring(file.name.lastIndexOf(".") + 1)) >= 0 ?
                            <Image avatar src={file.url} /> : null}
                        <List.Content>
                            {file.name}
                        </List.Content>
                    </List.Item>
                ))}
            </List>
        </Container>);
    };
}

export default Upload;
