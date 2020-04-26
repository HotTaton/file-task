import { MessageType } from './message-type.enum';

/**
 * Response message
 */
export class Message {
    constructor(public type: MessageType, public text: string) { }
}
