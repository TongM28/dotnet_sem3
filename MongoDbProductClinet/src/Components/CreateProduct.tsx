import { useState } from "react";
import { useNavigate } from "react-router-dom";
import API from "../API/api";
import { toast } from "react-toastify";

export const CreateProduct = () => {
    const navigate = useNavigate();

    const [formData, setFormData] = useState({
        name: "",
        description: "",
        price: "",
    });

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            await API.post("/Product", {
                ...formData,
                price: Number(formData.price),
            });
            toast.success("Product created successfully!");
            navigate("/");
        } catch (error: any) {
            const status = error.response?.status;
            const data = error.response?.data;

            if (!status) {
                toast.error("Network error or server is down");
                console.error("Network error or server is down");
                return;
            }

            let message = "An unexpected error occurred.";

            if (data && typeof data === "object") {
                message = data.detail || data.message || JSON.stringify(data);
            } else if (typeof data === "string") {
                message = data;
            }

            if (data && typeof data === "object") {
                message = data.detail || data.message || JSON.stringify(data);
            } else if (typeof data === "string") {
                message = data;
            }

            // Thêm trường hợp data rỗng nhưng status là 401
            if (status === 401 && !data) {
                message = "You must be logged in to perform this action.";
            }

            toast.error("Error creating product: " + message);
            console.error("Error details: ", error);
            console.error("Error response: ", error.response);
        }
    };

    return (
        <div className="max-w-2xl mx-auto px-4 py-8">
            <div className="bg-white rounded-lg shadow-lg p-6">
                <h2 className="text-2xl font-bold text-gray-800 mb-6">
                    Create New Product
                </h2>
                <form onSubmit={handleSubmit} className="space-y-4">
                    <div>
                        <label className="block text-gray-700 mb-2">Name</label>
                        <input
                            type="text"
                            value={formData.name}
                            onChange={(e) =>
                                setFormData({
                                    ...formData,
                                    name: e.target.value,
                                })
                            }
                            className="w-full px-3 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                            required
                        />
                    </div>
                    <div>
                        <label className="block text-gray-700 mb-2">
                            Description
                        </label>
                        <textarea
                            value={formData.description}
                            onChange={(e) =>
                                setFormData({
                                    ...formData,
                                    description: e.target.value,
                                })
                            }
                            className="w-full px-3 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                            rows={4}
                            required
                        />
                    </div>
                    <div>
                        <label className="block text-gray-700 mb-2">
                            Price
                        </label>
                        <input
                            type="number"
                            value={formData.price}
                            onChange={(e) =>
                                setFormData({
                                    ...formData,
                                    price: e.target.value,
                                })
                            }
                            className="w-full px-3 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                            required
                        />
                    </div>
                    <div className="flex space-x-4">
                        <button
                            type="submit"
                            className="bg-blue-500 hover:bg-blue-600 text-white px-4 py-2 rounded-md"
                        >
                            Create Product
                        </button>
                        <button
                            type="button"
                            onClick={() => navigate("/")}
                            className="bg-gray-500 hover:bg-gray-600 text-white px-4 py-2 rounded-md"
                        >
                            Cancel
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
};
