import { useState } from "react";
import { useNavigate } from "react-router-dom";
import API from "../API/api";
import { toast } from "react-toastify";

function Login() {
    const navigate = useNavigate();
    const [form, setForm] = useState({
        email: "",
        password: "",
    });

    const handleLogin = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const res = await API.post("/auth/login", form);
            console.log("Login response:", res.data);

            const token = res.data.token;
            localStorage.setItem("token", token);
            toast.success("Đăng nhập thành công!");
            console.log("Token saved:", token);
            navigate("/");
        } catch (error) {
            console.error("Login error:", error);
            toast.error("Wrong email or password!");
        }
    };

    return (
        <div className="max-w-md mx-auto px-4 py-8">
            <div className="bg-white rounded-lg shadow-lg p-6">
                <h2 className="text-2xl font-bold mb-6">Đăng nhập</h2>
                <form onSubmit={handleLogin} className="space-y-4">
                    <input
                        type="email"
                        placeholder="Email"
                        value={form.email}
                        autoFocus
                        onChange={(e) =>
                            setForm({ ...form, email: e.target.value })
                        }
                        className="w-full border px-3 py-2 rounded-md"
                        required
                    />
                    <input
                        type="password"
                        placeholder="Password"
                        value={form.password}
                        onChange={(e) =>
                            setForm({ ...form, password: e.target.value })
                        }
                        className="w-full border px-3 py-2 rounded-md"
                        required
                    />
                    <button
                        type="submit"
                        className="w-full bg-blue-500 text-white py-2 rounded-md hover:bg-blue-600"
                    >
                        Đăng nhập
                    </button>
                </form>
            </div>
        </div>
    );
}

export default Login;
