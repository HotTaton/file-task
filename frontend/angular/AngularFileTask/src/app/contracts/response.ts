import { Message } from './message';

/**
 * Contains service resposnse
 */
export class ServiceResponse<T> {
    constructor(public item: T, public messages: Message[], public hasErrors: boolean) { }
}
