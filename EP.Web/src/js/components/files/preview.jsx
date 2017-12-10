import React, { Component } from "react";
import { render } from 'react-dom';
import { Container, Card, Image } from 'semantic-ui-react';
import API from '../api';

class Preview extends Component {
    constructor(props) {
        super(props);
        this.state = {
            url: API.getBaseUri() + 'admin/blobManager/' + this.props.match.params.id
        }
    }

    render() {
        return (<Container >
            <Card>
                <Image src={this.state.url} fluid />
            </Card>
        </Container>);
    };
}

export default Preview;
