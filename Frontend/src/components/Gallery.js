import React, {useState,useEffect} from "react";
import GalleryGrid from "./GalleryGrid";
import { ENDPOINTS } from "../utils/consts";
import axiosAuth from "../utils/authInstance";

const Gallery = () => {
    const [images,setImages] = useState([]);
    const [isLoading,setIsLoading] = useState(true);
    useEffect(() => {
        const fetchData = async () => {
        try {
        const response = await axiosAuth.get(ENDPOINTS.GETGALLERY)
        setImages(response.data);
        setIsLoading(false);
        } catch (error) {
            console.error(error);
        }
    };

    fetchData();

    }, []);

    return (
        <div>
            <h1>Galeria</h1>
            {isLoading ? null : <GalleryGrid images={images} />} 
            
        </div>
    );
};

export default Gallery;
