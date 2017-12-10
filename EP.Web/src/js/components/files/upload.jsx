import React, { Component } from "react";
import { render } from 'react-dom';
import { Container, Button, Image, Form, List } from 'semantic-ui-react';
import API from '../api';

const styles = {
    marginTop: '5em',
    marginLeft: '0.5em'
};

class Upload extends Component {
    constructor(props) {
        super(props);
        this.state = {
            files: []
        }

        this.supportedImages = ['png', 'jpg', 'gif', 'jpeg']

        this.onUpload = this.onUpload.bind(this);
        this.removeItem = this.removeItem.bind(this);
        this.submit = this.submit.bind(this);
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

    removeItem(file) {
        this.setState((prevState, props) => {
            var arrayvar = prevState.files.slice();
            return { files: arrayvar.filter(el => el != file) };
        });
    }

    submit() {
        let body = new FormData();
        this.state.files.map(file => {
            body.append("files", file);
        });

        let header = new Headers({
            'Access-Control-Allow-Origin': '*',
            'Content-Type': 'multipart/form-data'
        });

        fetch(API.getBaseUri() + 'admin/blobManager', {
            method: 'post',
            header: header,
            body: body
        })
            .then(response => { return response.json(); })
            .then((json) => {
                this.props.onUploaded();
                this.setState((prevState, props) => {
                    return { files: [] };
                });
            })
            .catch(error => { console.log('request failed', error); });
    }

    render() {
        let fileInput = null;
        const uid = 'testinput';
        const color = 'red';
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
                            this.onUpload(fileInput.files[0]);
                        }}
                        ref={input => {
                            fileInput = input;
                        }}
                    />
                    <Form.Button
                        onClick={() => this.submit()}
                        style={styles}
                        disabled={this.state.files.length == 0}>Upload all</Form.Button>
                </Form.Group>
            </Form>
            <List divided verticalAlign='middle'>
                {this.state.files.map(file => (
                    <List.Item key={file.name}>
                        <List.Content floated='right'>
                            <Button onClick={() => this.removeItem(file)}>Remove</Button>
                        </List.Content>
                        {this.supportedImages.indexOf(file.name.substring(file.name.lastIndexOf(".") + 1)) >= 0 ?
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
