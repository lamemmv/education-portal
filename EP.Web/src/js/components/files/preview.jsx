import React, { Component } from "react";
import { render } from 'react-dom';
import { Container, Image } from 'semantic-ui-react';
import API from '../api';

class Preview extends Component {
    constructor(props) {
        super(props);
        this.state = {
            file: {}
        }

        this.getFile();
    }

    getFile() {
        let url = API.getBaseUri() + 'admin/blobManager/' + this.props.match.params.id;
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
                    return { file: response };
                });
            })
            .catch(error => { console.log('request failed', error); });
    }

    render() {
        return (<Container >
            preview
            <Image avatar src={this.state.file.url} />
        </Container>);
    };
}

export default Preview;
