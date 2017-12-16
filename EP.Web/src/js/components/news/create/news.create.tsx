import * as React from 'react';
import { render } from 'react-dom';
import { Link, Redirect } from 'react-router-dom';
import { RouteComponentProps } from 'react-router';

import { Container, Form, Input, TextArea, Button } from 'semantic-ui-react';

export interface Props extends RouteComponentProps<void> {

}

class CreateNews extends React.Component<Props, {}> {

    render() {
        return (
            <Container>
                <Form style={{ marginTop: '5em' }}>
                    <Form.Group widths='equal'>
                        <Form.Field id='form-input-control-first-name' control={Input} label='Title' placeholder='Title' />
                        <Form.Field id='form-input-control-last-name' control={Input} label='Ingress' placeholder='Ingress' />
                    </Form.Group>
                    <Form.Field id='form-textarea-control-opinion' control={TextArea} label='Content' placeholder='Content' />
                    <Form.Field id='form-button-control-public' control={Button} content='Create' />
                </Form>
            </Container>
        );
    }
}

export default CreateNews;
