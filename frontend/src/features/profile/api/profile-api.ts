import { api } from "@/lib";
import type { User } from "@/types/api";
import * as Endpoints from "@/lib/endpoints";

export const userInfo = async ( userSlug : string ) => {
    return await api.get<User>(Endpoints.GetUser.replace('{userSlug}', userSlug)); 
}